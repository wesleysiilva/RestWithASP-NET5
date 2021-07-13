﻿using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Model.Base;
using RestWithASPNET.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNET.Repository.Generic {
  public class GenericRepository<T> : IRepository<T> where T : BaseEntity {
    protected SqlServerContext _context;
    private DbSet<T> dataset;

    public GenericRepository(SqlServerContext context) {
      _context = context;
      dataset = _context.Set<T>();
    }

    public List<T> FindAll() {
      return dataset.ToList();
    }

    public T FindByID(long id) {
      return dataset.SingleOrDefault(p => p.Id.Equals(id));
    }

    public T Create(T item) {
      try {
        dataset.Add(item);
        _context.SaveChanges();
        return item;
      } catch (Exception e) {
        throw;
      }
    }

    public T Update(T item) {
      var result = dataset.SingleOrDefault(p => p.Id.Equals(item.Id));
      if (result != null) {
        try {
          _context.Entry(result).CurrentValues.SetValues(item);
          _context.SaveChanges();
          return result;
        } catch (Exception e) {
          throw;
        }
      }
      return null;
    }

    public void Delete(long id) {
      var result = dataset.SingleOrDefault(p => p.Id.Equals(id));
      if (result != null) {
        try {
          dataset.Remove(result);
          _context.SaveChanges();
        } catch (Exception e) {
          throw;
        }
      }
    }

    public bool Exists(long id) {
      return dataset.Any(p => p.Id.Equals(id));
    }

    public List<T> FindWithPagedSearch(string query) {
      return dataset.FromSqlRaw<T>(query).ToList();
    }

    public int GetCount(string query) {
      var result = "";

      using(var connection = _context.Database.GetDbConnection()) {
        connection.Open();
        using(var command = connection.CreateCommand()) {
          command.CommandText = query;
          result = command.ExecuteScalar().ToString();
        }
      }
      return int.Parse(result);
    }
  }
}
