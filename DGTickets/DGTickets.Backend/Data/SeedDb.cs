﻿using DGTickets.Backend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;

    public SeedDb(DataContext context, IFileStorage fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckStatesAsync();
        await CheckCitiesAsync();
        await CheckMedicinesAsync();
        await CheckHeadquartersAsync();
        await CheckRatingsAsync();
        await CheckModulesAsync();
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSQLScript = File.ReadAllText("Data\\Countries.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
            await CheckFlagsCountriesAsync();
        }
    }

    private async Task CheckStatesAsync()
    {
        if (!_context.States.Any())
        {
            var statesSQLScript = File.ReadAllText("Data\\States.sql");
            await _context.Database.ExecuteSqlRawAsync(statesSQLScript);
            await CheckFlagsStatesAsync();
        }
    }

    private async Task CheckCitiesAsync()
    {
        if (!_context.Cities.Any())
        {
            var citiesSQLScript = File.ReadAllText("Data\\Cities.sql");
            await _context.Database.ExecuteSqlRawAsync(citiesSQLScript);
            await CheckImagesCitiesAsync();
        }
    }

    private async Task CheckMedicinesAsync()
    {
        if (!_context.MedicinesStock.Any())
        {
            var medicinesSQLScript = File.ReadAllText("Data\\Medicines.sql");
            await _context.Database.ExecuteSqlRawAsync(medicinesSQLScript);
            await CheckImagesMedicineAsync();
        }
    }

    private async Task CheckHeadquartersAsync()
    {
        if (!_context.Headquarters.Any())
        {
            var headquartersSQLScript = File.ReadAllText("Data\\Headquarters.sql");
            await _context.Database.ExecuteSqlRawAsync(headquartersSQLScript);
        }
    }

    private async Task CheckRatingsAsync()
    {
        if (!_context.Ratings.Any())
        {
            var ratingsSQLScript = File.ReadAllText("Data\\Ratings.sql");
            await _context.Database.ExecuteSqlRawAsync(ratingsSQLScript);
        }
    }

    private async Task CheckModulesAsync()
    {
        if (!_context.Modules.Any())
        {
            var modulesSQLScript = File.ReadAllText("Data\\Modules.sql");
            await _context.Database.ExecuteSqlRawAsync(modulesSQLScript);
        }
    }

    private async Task CheckFlagsCountriesAsync()
    {
        foreach (var country in _context.Countries)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\Flags\\{country.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "countries");
            }

            country.Image = imagePath;
            _context.Entry(country).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckFlagsStatesAsync()
    {
        foreach (var state in _context.States)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\States\\{state.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "states");
            }

            state.Image = imagePath;
            _context.Entry(state).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckImagesMedicineAsync()
    {
        foreach (var medicine in _context.MedicinesStock)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\Medicines\\{medicine.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "medicines");
            }

            medicine.Image = imagePath;
            _context.Entry(medicine).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckImagesCitiesAsync()
    {
        foreach (var cities in _context.Cities)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\Cities\\{cities.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "cities");
            }

            cities.Image = imagePath;
            _context.Entry(cities).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }
}