﻿using System.Collections.Generic;
using MGroup.FEM.Entities;
using MGroup.MSolve.Discretization;
using MGroup.MSolve.Discretization.FreedomDegrees;
using MGroup.MSolve.Discretization.Interfaces;
using MGroup.Multiscale.Interfaces;

namespace MGroup.Multiscale.ScaleTransitions
{
    /// <summary>
    /// Micro to macro transitions for 3D problems
    /// Authors: Gerasimos Sotiropoulos
    /// </summary>
    public class SmallStrain3DScaleTransition : IScaleTransitions
    {
        public SmallStrain3DScaleTransition()
        { }

        public double[] MacroToMicroTransition(Node boundaryNode, double[] MacroScaleVariable)
        {
            double[,] Dq_nodal = new double[6,3]; // Prosoxh: pithanes diorthoseis eis triploun
            Dq_nodal[0, +0] = boundaryNode.X;
            Dq_nodal[1, +1] = boundaryNode.Y;
            Dq_nodal[2, +2] = boundaryNode.Z;

            Dq_nodal[3, +0] = 0.5*boundaryNode.Y;
            Dq_nodal[3, +1] = 0.5*boundaryNode.X;

            Dq_nodal[4, +1] = 0.5*boundaryNode.Z;
            Dq_nodal[4, +2] = 0.5*boundaryNode.Y;


            Dq_nodal[5, +0] = 0.5*boundaryNode.Z;
            Dq_nodal[5, +2] = 0.5*boundaryNode.X;


            double[] microVariable = new double[3];            

            for (int i1 = 0; i1 < 3; i1++)
            {
                for (int j1 = 0; j1 < 6; j1++)
                {
                    microVariable[i1] += Dq_nodal[j1, i1] * MacroScaleVariable[j1]; //einai sunolikh 
                }
            }

            return microVariable;
        }

        public double[] MicroToMacroTransition(INode boundaryNode, double[] MicroScaleVariable)
        {
            double[,] Dq_nodal = new double[6, 3]; // Prosoxh: pithanes diorthoseis eis triploun
            Dq_nodal[0, +0] = boundaryNode.X;
            Dq_nodal[1, +1] = boundaryNode.Y;
            Dq_nodal[2, +2] = boundaryNode.Z;

            Dq_nodal[3, +0] = 0.5 * boundaryNode.Y;
            Dq_nodal[3, +1] = 0.5 * boundaryNode.X;

            Dq_nodal[4, +1] = 0.5 * boundaryNode.Z;
            Dq_nodal[4, +2] = 0.5 * boundaryNode.Y;


            Dq_nodal[5, +0] = 0.5 * boundaryNode.Z;
            Dq_nodal[5, +2] = 0.5 * boundaryNode.X;

            double[] macroVariable = new double[6];
            //
            for (int i1 = 0; i1 < 6; i1++)
            {
                for (int j1 = 0; j1 < 3; j1++)
                {
                    macroVariable[i1] += Dq_nodal[ i1, j1] * MicroScaleVariable[j1]; //einai sunolikh 
                }
            }

            return macroVariable;
        }

        public int PrescribedDofsPerNode()
        {
            return 3;
        }

        public int MacroscaleVariableDimension()
        {
            return 6;
        }

        public void ModifyMicrostructureTotalPrescribedBoundaryDisplacementsVectorForMacroStrainVariable(INode boundaryNode,
            double[] smallStrain3Dmacro, Dictionary<int, Dictionary<IDofType, double>> totalPrescribedBoundaryDisplacements)
        {
            double[,] Dq_nodal = new double[6, 3]; // Prosoxh: pithanes diorthoseis eis triploun
            Dq_nodal[0, +0] = boundaryNode.X;
            Dq_nodal[1, +1] = boundaryNode.Y;
            Dq_nodal[2, +2] = boundaryNode.Z;

            Dq_nodal[3, +0] = 0.5 * boundaryNode.Y;
            Dq_nodal[3, +1] = 0.5 * boundaryNode.X;

            Dq_nodal[4, +1] = 0.5 * boundaryNode.Z;
            Dq_nodal[4, +2] = 0.5 * boundaryNode.Y;


            Dq_nodal[5, +0] = 0.5 * boundaryNode.Z;
            Dq_nodal[5, +2] = 0.5 * boundaryNode.X;

            //double[] thesi_prescr_xyz = new double[2];
            double[] u_prescr_xyz_sunol = new double[3];

            for (int i1 = 0; i1 < 3; i1++)
            {
                for (int j1 = 0; j1 < 6; j1++)
                {
                    u_prescr_xyz_sunol[i1] += Dq_nodal[j1, i1] * smallStrain3Dmacro[j1]; //einai sunolikh 
                }
            }

            //SHMEIWSH: an prosthesoume sto totalBoundaryNodalDIsplacements trith metakinhsh (dld logw u_prescr_xyz_sunol[3]) ==0
            // ousiastika h methodos "ImposePrescribedDisplacementsWithInitialConditionSEffect" ths subdomain.cs tha paei kai tha 
            //xanagrapsei panw apo to 0 tou localsolution enos element (pou prokuptei ean exoume thesei sthnepomenh methodo dof = constrained) thn timh 0
            //pou tha vrei sto totalBoundaryNodalDIsplacements. an ekei den exoume valei timh (dld logw u_prescr_xyz_sunol[3] ==0
            //sto  DOFType.Z tou Dictionary) den tha peiraxei to mhden tou localsolution opote einai to idio.

            Dictionary<IDofType, double> totalBoundaryNodalDisplacements = new Dictionary<IDofType, double>();
            totalBoundaryNodalDisplacements.Add(StructuralDof.TranslationX, u_prescr_xyz_sunol[0]);
            totalBoundaryNodalDisplacements.Add(StructuralDof.TranslationY, u_prescr_xyz_sunol[1]);
            totalBoundaryNodalDisplacements.Add(StructuralDof.TranslationZ, u_prescr_xyz_sunol[2]);
            

            totalPrescribedBoundaryDisplacements.Add(boundaryNode.ID, totalBoundaryNodalDisplacements);
        }

        public void ImposeAppropriateAndRigidBodyConstraintsPerBoundaryNode(Model model, Node boundaryNode, Dictionary<Node, IList<IDofType>> RigidBodyNodeConstraints)
        {
            throw new System.NotSupportedException();
        }

        public void ImposeAppropriateConstraintsPerBoundaryNode(Model model, Node boundaryNode)
        {

            model.NodesDictionary[boundaryNode.ID].Constraints.Add(new Constraint() {DOF=StructuralDof.TranslationX});
            model.NodesDictionary[boundaryNode.ID].Constraints.Add(new Constraint() { DOF = StructuralDof.TranslationY });
            model.NodesDictionary[boundaryNode.ID].Constraints.Add(new Constraint() { DOF = StructuralDof.TranslationZ });
           
        }
    }
}
