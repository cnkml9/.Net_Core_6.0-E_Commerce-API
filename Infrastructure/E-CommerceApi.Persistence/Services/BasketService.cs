using E_CommerceApi.Application.Abstractions.Services;
using E_CommerceApi.Application.Repositories;
using E_CommerceApi.Application.ViewModels.Baskets;
using E_CommerceApi.Domain.Entities;
using E_CommerceApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _contextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;

        public BasketService(IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketReadRepository)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemReadRepository = basketReadRepository;
        }


        private async Task<Basket?> ContexctUser()
        {
            var userName =  _contextAccessor?.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
              AppUser? user =  await _userManager.Users
                    .Include(u=>u.Baskets)
                    .FirstOrDefaultAsync(u=>u.UserName == userName);

                var _basket = from basket in user?.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into BasketOrders
                              from orderB in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                 Basket =  basket,
                                 Order = orderB
                              };


                Basket? targetBasket = null;
                if(_basket.Any(b=>b.Order is null)) {
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else
                {
                    targetBasket = new();
                    user?.Baskets.Add(targetBasket);
                }

                await _basketWriteRepository.SaveAsync();
                return targetBasket;
            }
            throw new Exception("Beklenmeyen bir hata ile karşılaşıldı");
             
        }

        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContexctUser();
            if(basket != null)
            {
                _basketItemReadRepository.
            }
        }

        public Task<List<BasketItem>> GetBasketItemAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveBasketItem(string basketItemId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            throw new NotImplementedException();
        }
    }
}
