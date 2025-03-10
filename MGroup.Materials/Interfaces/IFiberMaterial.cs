﻿namespace MGroup.Materials.Interfaces
{
    public interface IFiberMaterial : IFiniteElementMaterial
    {
        double Stress { get; }
        double Strain { get; }
        void UpdateMaterial(double dStrain);
        void SaveState();
        void ClearStresses();
        IFiberMaterial Clone(IFiberFiniteElementMaterial parent);
        double YoungModulus { get; set; }
        double PoissonRatio { get; set; } //It might be useless
        //double YoungModulusElastic { get; set; }
    }
}
