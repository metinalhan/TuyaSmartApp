using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Dtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Persistence.Services
{
    public class DashboardService : IDashboardService
    {
        IDashboardRepository _repository;
        IMapper _mapper;

        //DI method for inject repository and mapper
        public DashboardService(IDashboardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //This method getting all dashboard devices from db
        public async Task<List<Dashboard>> GetAllDashboardDevicesAsync()
        {
           return await _repository.GetAll().ToListAsync();
        }

        //This method saving all dashboard devices to db
        public async Task<bool> SaveDashboardAsync(List<DashboardSaveDTO> list)
        {
            var mapped = _mapper.Map<List<Dashboard>>(list);

            var removeAll = await GetAllDashboardDevicesAsync();
            _repository.RemoveRange(removeAll);

            await _repository.AddRangeAsync(mapped);
            await _repository.SaveAsync();

            return true;
        }
    }
}
