using System.ComponentModel.DataAnnotations;
using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.DTOs.Response;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;
using RevenueRecognitionSystem.Repositories;

namespace RevenueRecognitionSystem.Services;

public class ClientsService : IClientsService
{
    private readonly IClientRepository _clientsRepository;
    
    public ClientsService(IClientRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task AddClientAsync(CancellationToken token, AddClientRequestDto dto)
    {
        if (dto.ClientType == "individual")
        {
            if(string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName) || string.IsNullOrWhiteSpace(dto.Pesel))
            {
                throw new ValidationException("First name, last name, and PESEL are required for individual clients.");
            }

            var individual = new Individual()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Pesel = dto.Pesel,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            
            await _clientsRepository.AddClientAsync(token, individual);
        }
        else if (dto.ClientType == "company")
        {
            if(string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Krs))
            {
                throw new ValidationException("Name and KRS are required for company clients.");
            }

            var company = new Company()
            {
                Name = dto.Name,
                Krs = dto.Krs,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            
            await _clientsRepository.AddClientAsync(token, company);
        }
        else
        {
            throw new ValidationException("Invalid client type. Must be 'individual' or 'company'.");
        }
    }

    public async Task<List<ClientResponseDto>> GetClientsAsync(CancellationToken token)
    {
        var individuals = await _clientsRepository.GetAllIndividualsAsync(token);
        var companies = await _clientsRepository.GetAllCompaniesAsync(token);

        var individualDtos = individuals.Select(i => new ClientResponseDto
        {
            ClientType = "Individual",
            Address = i.Address,
            Email = i.Email,
            PhoneNumber = i.PhoneNumber,
            FirstName = i.FirstName,
            LastName = i.LastName,
            Pesel = i.Pesel
        });

        var companyDtos = companies.Select(c => new ClientResponseDto
        {
            ClientType = "Company",
            Address = c.Address,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            CompanyName = c.Name,
            Krs = c.Krs
        });

        return individualDtos.Concat(companyDtos).OrderBy(c => c.ClientType).ToList();
    }

    public async Task<ClientResponseDto> GetClientByIdAsync(CancellationToken token, int id)
    {
        var client = await _clientsRepository.GetClientByIdAsync(token, id);

        if (client == null)
        {
            throw new NotFoundException($"Client with id {id} not found");
        }

        return client switch
        {
            Individual individual => new ClientResponseDto
            {
                ClientType = "Individual",
                Address = individual.Address,
                Email = individual.Email,
                PhoneNumber = individual.PhoneNumber,
                FirstName = individual.FirstName,
                LastName = individual.LastName,
                Pesel = individual.Pesel
            },
            Company company => new ClientResponseDto
            {
                ClientType = "Company",
                Address = company.Address,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                CompanyName = company.Name,
                Krs = company.Krs
            },
            _ => throw new Exception("Unknown client type.")
        };
    }
    
    public async Task UpdateClientAsync(CancellationToken token, int id, UpdateClientRequestDto dto)
    {
        var client = await _clientsRepository.GetClientByIdAsync(token, id)
                     ?? throw new NotFoundException($"Client with ID {id} not found.");

        if (!string.IsNullOrWhiteSpace(dto.Address)) client.Address = dto.Address;
        if (!string.IsNullOrWhiteSpace(dto.Email)) client.Email = dto.Email;
        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber)) client.PhoneNumber = dto.PhoneNumber;

        switch (client)
        {
            case Individual individual:
                if (!string.IsNullOrWhiteSpace(dto.FirstName)) individual.FirstName = dto.FirstName;
                if (!string.IsNullOrWhiteSpace(dto.LastName)) individual.LastName = dto.LastName;
                break;

            case Company company:
                if (!string.IsNullOrWhiteSpace(dto.CompanyName)) company.Name = dto.CompanyName;
                break;
        }

        await _clientsRepository.UpdateClientAsync(token, client);
    }

    public async Task DeleteClientAsync(CancellationToken token, int id)
    {
        var client = await _clientsRepository.GetClientByIdAsync(token, id)
                     ?? throw new NotFoundException($"Client with ID {id} not found.");

        await _clientsRepository.DeleteClientAsync(token, client.Id);
    }
}