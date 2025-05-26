using System.ComponentModel.DataAnnotations;

namespace ThreeBodySimulation.Blazor.Core;

public enum SolverType
{
    [Display(Name = "Runge-Kutta (4th order)")]
    RK4,
    [Display(Name = "Yoshida (4th order)")]
    Yoshida4
}
