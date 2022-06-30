using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevInSales.Testes.Services
{
    public class StateServiceTeste
    {
        private DataContext _context;
        private StateService _service;

        public StateServiceTeste()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _service = new StateService(_context);
        }

        [Theory]
        [InlineData("Acre")]
        public void GetAll_ObterLista_RetornaLista(string? nome) 
        {
            Seeds();
            var expected = _context.States.Where(e => e.Name.ToUpper().Contains(nome.ToUpper())).Select(x => ReadState.StateToReadState(x)).ToList();
            var resultado = _service.GetAll(nome);

            Assert.Equal(expected.Count, resultado.Count);
            Assert.Equal(expected.FirstOrDefault().Name, resultado.FirstOrDefault().Name) ;
        }
        
        [Theory]
        [InlineData(null)]
        public void GetAll_ParametroNull_RetornaLista(string? nome)
        {
            Seeds();
            //var expected = _context.States.Where(e => e.Name.ToUpper().Contains(nome.ToUpper())).Select(x => ReadState.StateToReadState(x)).ToList();
            var resultado = _service.GetAll(nome);

            Assert.NotNull(resultado);
            Assert.Equal(3,resultado.Count);
        }

        [Theory]
        [InlineData(1)]
        public void GetById_PassandoId_RetornarEstado(int id)
        {
            Seeds();
            var expected = _context.States.FirstOrDefault(s => s.Id == id);
            var resultado = _service.GetById(id);

            Assert.Equal(expected.Name, resultado.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public void GetById_IdMenorQueZero_RetornarNull(int id)
        {
            Seeds();
            var resultado = _service.GetById(id);
            Assert.Null(resultado);
        }

        public void Seeds()
        {
            _context.States.AddRange(
               new List<State>()
               {
                   new State(1,"Acre","AC"),
                   new State(2,"Alagoas", "AL"),
                   new State(3,"Amazonas","AM")
               }
            );
            _context.SaveChanges();
        }
    }
}
