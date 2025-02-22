﻿using System.Collections.Generic;
using MGroup.FEM.Entities;
using MGroup.MSolve.Discretization.FreedomDegrees;

namespace MGroup.Multiscale.Interfaces
{
    /// <summary>
    /// Indicates additional methods that should be implemented by rveBuilders that will be used in FE2 3D to 2D degenerate analysis
    /// Authors: Gerasimos Sotiropoulos
    /// </summary>
    public interface IdegenerateRVEbuilder : IRVEbuilder
    {
        Dictionary<Node, IList<IDofType>> GetModelRigidBodyNodeConstraints(Model model);
    }
}
