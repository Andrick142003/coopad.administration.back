namespace Coopad.Administration.Api.DTOs.Responses;

public sealed record SorteoNumeroResponse(
    int Id,
    int Numero,
    string Tipo,
    bool Estado
);