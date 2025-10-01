using AutoMapper;
using CoreWebApiSuperHero.Data;

namespace CoreWebApiSuperHero.Configurations
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap(Source,Destination);
            CreateMap<Student, Models.StudentDTO>().ForMember(x=>x.StudentName,opt=>opt.MapFrom(x=>x.StudentName)).ReverseMap();  // .ForMember is used to map specific properties when the property names differ between source and destination

            //to ignore mapping of a specific property use .ForMember(dest=>dest.Property,opt=>opt.Ignore())
            // CreateMap<Student, Models.StudentDTO>().ForMember(x => x.DOB, opt => opt.Ignore()).ReverseMap();
            //config for null values in source object then add some message in destination object 

            // AddTransform is used to transform the source value before mapping it to the destination property
            //CreateMap<Models.StudentDTO, Student>().ReverseMap().AddTransform<string>(src => src == null ? "Email address not found" : src);
            
            //config for null or empty values in perticular source prperty then add some message in destination object
            CreateMap<Models.StudentDTO, Student>().ReverseMap()
            .ForMember(x=>x.Address,opt=>opt.MapFrom(src => string.IsNullOrEmpty(src.Address)?"Address not found":src.Address) );

            CreateMap<Models.RoleDTO, Role>().ReverseMap();
            CreateMap<Models.RolePrivilegeDTO, RolePrivilege>().ReverseMap();
            CreateMap<Models.UserDTO, User>().ReverseMap();
            CreateMap<Models.CompanyDTO, Data.EntityModel.Company>().ReverseMap();
            CreateMap<Models.EmployeeDTO, Data.EntityModel.Employee>().ReverseMap();
        }
    }
}
