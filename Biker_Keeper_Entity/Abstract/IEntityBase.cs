using System;
namespace Biker_Keeper_Data
{
    public interface IEntityBase
    {
        int Id { get; set; }
        bool? IsEnabled { get; set; }
    }
}
