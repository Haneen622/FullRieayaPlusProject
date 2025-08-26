//using AdvSwProject.Models;
//using AdvSwProject.ViewModels;


//namespace AdvSwProject.Extenshions.MappingExtenshion
//{
//    public static class AccountMappingExtenshion
//    {
//        public static User toModel(this UserViewmodel model)
//        {
//            return new User
//            {
//                Id = model.Id,
//                Username = model.Username,
//                Email = model.Email,
//                Password = model.Password
//            };

//        }
//        public static UserViewmodel toViewModel(this User model)
//        {

//            return new UserViewmodel
//            {

//                Id = model.Id,
//                Username = model.Username,
//                Email = model.Email,
//                Password = model.Password

//            };


//        }

//        public static List<User> toModelList(this List<UserViewmodel> model)
//        {

//            return model.Select(x => x.toModel()).ToList();


//        }
//        public static List<UserViewmodel> toViewModelList(this List<User> model)
//        {
//            return model.Select(x => x.toViewModel()).ToList();

//        }
//    }
//}

using AdvSwProject.Models;
using AdvSwProject.ViewModels;
using System.Linq;

namespace AdvSwProject.Extenshions.MappingExtenshion
{
    public static class AccountMappingExtenshion
    {
        public static User toModel(this UserViewmodel model)
        {
            return new User
            {
                Id = model.Id,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };
        }

        public static UserViewmodel toViewModel(this User model)
        {
            return new UserViewmodel
            {
                Id = model.Id,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };
        }
    }
}
