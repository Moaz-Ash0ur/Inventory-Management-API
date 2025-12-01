using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Inventory.BLL.DTOs;
using Inventory.BLL.Interface.Base;
using Inventory.DAL.Contracts;
using Inventory.DAL.Models;
using Inventory.Domains;

namespace Inventory.BLL.Interface
{
    public interface ICategories : IBaseService<Category, CategoryDto>
    {
        // دوال إضافية لو في عمليات خاصة بالـ Category
    }



}






