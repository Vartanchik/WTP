//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WTP.DAL.Entities;
//using WTP.DAL.Repositories.GenericRepository;

//namespace WTP.DAL.Repositories.ConcreteRepositories
//{
//    internal class CountryRepository : RepositoryBase<Country>, IRepository<Country>
//    {
//        private readonly ApplicationDbContext _context;

//        public CountryRepository(ApplicationDbContext context) : base(context)
//        {
//            _context = context;
//        }

//        public override async Task CreateAsync(Country item)
//        {
//            await _context.Countries.AddAsync(item);

//            await _context.SaveChangesAsync();
//        }

//        public override async Task UpdateAsync(Country item)
//        {
//            _context.Entry(item).State = EntityState.Modified;

//            await _context.SaveChangesAsync();
//        }

//        public override async Task DeleteAsync(int id)
//        {
//            var entity = await GetAsync(id);

//            if (entity != null)
//            {
//                _context.Entry(entity).State = EntityState.Deleted;

//                await _context.SaveChangesAsync();
//            }
//        }

//        public override async Task<Country> GetAsync(int id)
//        {
//            return await _context.Countries.FirstOrDefaultAsync(_ => _.Id == id);
//        }

//        public override async Task<IEnumerable<Country>> GetAllAsync()
//        {
//            return await _context.Countries.ToListAsync();
//        }
//    }
//}
