using System;
using Play.Catalog.Service.Entities;
using static Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service;

public static class Extensions
{
    public static ItemDto AsDto(this Items items)
    {
        return new ItemDto(items.Id, items.Name, items.Description, items.Price, items.CreatedDate);
    }

}
