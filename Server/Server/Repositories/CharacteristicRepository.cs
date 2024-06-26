﻿using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class CharacteristicRepository : ICharacteristicRepository
    {
        private readonly ApplicationDbContext _context;

        public CharacteristicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CharacteristicEntity> GetAll()
        {
            return _context.Characteristics.ToList();
        }

        public CharacteristicEntity GetById(int id)
        {
            return _context.Characteristics.Find(id);
        }

        public void Insert(CharacteristicEntity characteristic)
        {
            _context.Characteristics.Add(characteristic);
        }

        public void Update(CharacteristicEntity characteristic)
        {
            Delete(characteristic.Id);
            Save();
            Insert(characteristic);
        }

        public void Delete(Guid id)
        {
            CharacteristicEntity characteristic = _context.Characteristics.Find(id);
            if (characteristic != null)
            {
                _context.Characteristics.Remove(characteristic);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
