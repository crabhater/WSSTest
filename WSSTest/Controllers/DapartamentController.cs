using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Xml.Linq;
using WSSTest.DataContext;
using WSSTest.InterfacesLib;
using WSSTest.Models;
using System.Xml.Serialization;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace WSSTest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DapartamentController : ControllerBase
    {
        private IRepository<Company> _companyRepository;
        private IRepository<Departament> _departamentRepository;
        private IRepository<Group> _groupRepository;
        public DapartamentController(IRepository<Company> companyRep, IRepository<Departament> deptRep, IRepository<Group> groupRep)
        {
            _companyRepository = companyRep;
            _departamentRepository = deptRep;
            _groupRepository = groupRep;
        }
        [HttpGet]
        public async Task<IActionResult> CreateData()
        {
            var company = new Company()
            {
                Name = "company1"
            };
            await _companyRepository.CreateAsync(company);

            var departament = new Departament()
            {
                Name = "dept",
                CompanyId = company.Id,
            };
            var departament1 = new Departament()
            {
                Name = "dept1",
                CompanyId = company.Id,

            };
            var departament2 = new Departament()
            {
                Name = "dept2",
                CompanyId = company.Id,

            };
            var departament3 = new Departament()
            {
                Name = "dept3",
                CompanyId = company.Id,

            };
            var departament4 = new Departament()
            {
                Name = "dept4",
                CompanyId = company.Id,

            };

            await _departamentRepository.CreateAsync(new List<Departament> { departament, departament1, departament2, departament3, departament4});

            var group = new Group()
            {
                Name = "group",
                DepartamentId = departament.Id,
            };
            var group1 = new Group()
            {
                DepartamentId = departament.Id,
                Name = "group1",
            };
            var group2 = new Group()
            {
                DepartamentId = departament1.Id,
                Name = "group2"
            };
            var group3 = new Group()
            {
                DepartamentId = departament1.Id,
                Name = "group3"
            };

            await _groupRepository.CreateAsync(new List<Group> { group, group1, group2, group3});

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<string>> ExportXml()
        {
            var company = await _companyRepository.GetList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Company>));
            var stringBuilder = new StringBuilder();
            using TextWriter writer = new StringWriter(stringBuilder);
            serializer.Serialize(writer, company);
            var str = stringBuilder.ToString();
            return Ok(str);
        }

        [HttpPost]
        public async Task<IActionResult> ImportXml(string xstr)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Company));
                using var textreader = new StringReader(xstr);
                var company = serializer.Deserialize(textreader);
                await _companyRepository.CreateAsync((Company)company);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }

            return Ok();
        }
        [HttpPatch]
        public async Task<IActionResult> MoveGroup(int groupId, int deptId)
        {
            var group = await _groupRepository.GetAsync(groupId);
            if (group.DepartamentId != deptId)
            {
                group.DepartamentId = deptId;
                await _groupRepository.UpdateAsync(group);
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            if (group != null && group.DepartamentId != null)
            {
                await _groupRepository.CreateAsync(group);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(Group group)
        {
            if(group != null)
            {
                var existing = await _groupRepository.GetAsync(group.Id);
                if (existing != null)
                {
                    await _groupRepository.DeleteAsync(existing);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
