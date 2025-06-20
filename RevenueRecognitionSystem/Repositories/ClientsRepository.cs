﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public class ClientsRepository : IClientRepository
{
    private readonly AppDbContext _context;
    
    public ClientsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddClientAsync(CancellationToken token, Individual individual)
    {
        _context.Clients.Add(individual);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task AddClientAsync(CancellationToken token, Company company)
    {
        _context.Clients.Add(company);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task<IEnumerable<Individual>> GetAllIndividualsAsync(CancellationToken token)
    {
        return await _context.Individuals
            .Where(i => i.Pesel != null)
            .ToListAsync(token);
    }
    
    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(CancellationToken token)
    {
        return await _context.Companies
            .Where(i => i.Krs != null)
            .ToListAsync(token);
    }
    
    public async Task<Client?> GetClientByIdAsync(CancellationToken token, int id)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, token);
    }


    public async Task UpdateClientAsync(CancellationToken token, Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task DeleteClientAsync(CancellationToken token, int id)
    {
        var client = await GetClientByIdAsync(token, id);
        if (client == null)
        {
            throw new NotFoundException($"Client with id {id} not found.");
        }

        if (client is Individual individual)
        {
            individual.FirstName = "[deleted]";
            individual.LastName = "[deleted]";
            individual.Pesel = "[deleted]";
            individual.Email = "[deleted]";
            individual.PhoneNumber = "[deleted]";
            individual.Address = "[deleted]";
            individual.IsDeleted = true;
    
            _context.Clients.Update(individual);
            await _context.SaveChangesAsync(token);
        }
        else if (client is Company)
        {
            throw new ValidationException("Deleting company clients is not allowed.");
        }
    }
}