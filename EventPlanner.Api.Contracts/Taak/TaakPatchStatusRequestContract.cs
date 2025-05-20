using System;
using EventPlanner.Shared;

namespace EventPlanner.Api.Contracts.Taak;

public class TaakPatchStatusRequestContract
{
    public StatusEnum Status { get; set; }
}
