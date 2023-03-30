using MagicVilla_VillaAPI.Model.DTO;

namespace MagicVilla_VillaAPI.Data
{
    public static class Villastore
    {
            public static List<VillaDTO> ListVilla= new List<VillaDTO>
            {
                new VillaDTO { Id=1, Name="Desert View",Sqmeter="100", Occupancy="3"},
                new VillaDTO { Id=2, Name="Beach View",Sqmeter="200",Occupancy="4" }
            };
    }
}
