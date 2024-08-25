namespace Fiap.TechChallenge.Domain.Entities;

public class DddState(short dddNumber, string stateName, string stateAbbreviation)
{
    public short  DddNumber { get; init; } = dddNumber;
    public string  StateName { get; init; } = stateName;
    public string  StateAbbreviation { get; init; } = stateAbbreviation;
}