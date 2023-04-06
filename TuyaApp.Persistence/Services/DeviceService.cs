using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Application.Dtos;
using TuyaApp.Application.Dtos.DeviceDtos;
using TuyaApp.Domain.Entities;

namespace TuyaApp.Persistence.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IMapper _mapper;

        // Dependency injection
        public DeviceService(IDeviceRepository deviceRepository, IMapper mapper)
        {
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        // Add new device
        public async Task AddNewDeviceAsync(CreateDeviceDTO device)
        {
            // Create new device
            var new_device = new Device
            {
                DeviceName = device.DeviceName,
                DeviceType = device.DeviceType,
                DeviceTuyaId = device.DeviceTuyaId,
                NumberOfSwitch = device.NumberOfSwitch,
                DefaultFunction = "",
                TuyaAccount = device.TuyaAccount
            };

            // Add and save device
            await _deviceRepository.AddAsync(new_device);
            await _deviceRepository.SaveAsync();
        }


        // Delete device by id
        public async Task<bool> DeleteDeviceAsync(int deviceId)
        {
            await _deviceRepository.RemoveAsync(deviceId);
            await _deviceRepository.SaveAsync();

            return true;
        }

        // Get all devices
        public async Task<List<Device>> GetAllDevicesAsync() =>        
          await _deviceRepository.GetAll().ToListAsync();

        // Get default device
        public async Task<Device> GetDefaultDeviceAsync()=>        
            await _deviceRepository.GetAll(x=>x.IsDefault).FirstOrDefaultAsync();

        // Get device by id
        public async Task<Device> GetDeviceByIdAsync(int deviceId)=>
             await _deviceRepository.GetById(deviceId).FirstOrDefaultAsync();

        // Get all devices by account id
        public async Task<List<DeviceDTO>> GetDevicesByAccountAsync(int account_id)
        {
           var result = await _deviceRepository.GetAll(x => x.TuyaAccount.Id == account_id).ToListAsync();
            return _mapper.Map<List<DeviceDTO>>(result);
        }

        // Make device favourite
        public async Task<bool> MakeFavouriteDeviceAsync(int deviceId)
        {
            var list = await GetAllDevicesAsync();

            foreach (var item in list)
            {
                if (item.Id != deviceId)
                    item.IsDefault = false;
                else
                    item.IsDefault = true;
            }

           await _deviceRepository.SaveAsync();
            return true;
        }

        // Assign default function to device
        public async Task<bool> AssignDefaultFunctionToDeviceAsync(Device device)
        {
            _deviceRepository.Update(device);
          return await MakeFavouriteDeviceAsync(device.Id);
        }
    }
}
