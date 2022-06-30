using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class StateService : IStateService
    {
        private readonly DataContext _context;

        public StateService(DataContext context)
        {
            _context = context;
        }

        public List<ReadState> GetAll(string? name)
        {
            return _context.States.Include(p => p.Cities)
                .Where(p => !String.IsNullOrWhiteSpace(name)? p.Name.ToUpper().Contains(name.ToUpper()): true)
                .Select(s => ReadState.StateToReadState(s))
                .ToList();
        }

        public ReadState GetById(int stateId)
        {
            var state = _context.States.Include(p => p.Cities).FirstOrDefault(p => p.Id == stateId);
            return ReadState.StateToReadState(state);
        }
    }
}
