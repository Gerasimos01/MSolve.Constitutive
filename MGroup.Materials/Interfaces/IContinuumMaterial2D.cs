﻿using MGroup.LinearAlgebra.Matrices;

namespace MGroup.Materials.Interfaces
{
    public interface IContinuumMaterial2D : IFiniteElementMaterial
    {
        double[] Stresses { get; }
        IMatrixView ConstitutiveMatrix { get; }
        void UpdateMaterial(double[] strains);
    }
}
