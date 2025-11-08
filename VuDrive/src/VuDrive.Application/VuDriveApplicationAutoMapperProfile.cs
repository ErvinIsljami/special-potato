using AutoMapper;
using VuDrive.Cars;
using VuDrive.Displays;
using VuDrive.ProductSets;

namespace VuDrive;

public class VuDriveApplicationAutoMapperProfile : Profile
{
    public VuDriveApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<VuDrive.Cars.Car, CarDto>();
        CreateMap<CreateUpdateCarDto, VuDrive.Cars.Car>();

        CreateMap<VuDrive.Displays.Display, DisplayDto>();
        CreateMap<CreateUpdateDisplayDto, VuDrive.Displays.Display>();

        CreateMap<VuDrive.ProductSets.ProductSet, ProductSetDto>();
        CreateMap<CreateUpdateProductSetDto, VuDrive.ProductSets.ProductSet>();

    }
}
