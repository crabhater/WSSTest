using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Xml.Serialization;
using WSSTest.Controllers;
using WSSTest.DataContext;
using WSSTest.Models;
using Xunit;
using Group = WSSTest.Models.Group;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;
using OkResult = Microsoft.AspNetCore.Mvc.OkResult;

public class DepartamentControllerTests
{
    private readonly EFCompanyRepository _companyRepositoryMock;
    private readonly EFDepartamentRepository _departamentRepositoryMock;
    private readonly EFGroupRepository _groupRepositoryMock;
    private readonly DapartamentController _controller;
    private readonly CompanyContext _context;

    public DepartamentControllerTests()
    {
        var options = new DbContextOptionsBuilder<CompanyContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WSSTestDB;Trusted_Connection=True;")
            .Options;
        _context = new CompanyContext(options);
        _companyRepositoryMock = new EFCompanyRepository(_context);
        _departamentRepositoryMock = new EFDepartamentRepository(_context);
        _groupRepositoryMock = new EFGroupRepository(_context);
        _controller = new DapartamentController(_companyRepositoryMock, _departamentRepositoryMock, _groupRepositoryMock);
    }

    [Fact]
    public async Task ExportXml_ReturnsOkResult_WithXmlString()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        // Arrange
        var companies = new List<Company> { new Company { Name = "Company1", Child = new List<Departament>() } };
        await _companyRepositoryMock.CreateAsync(companies);
        var serializer = new XmlSerializer(typeof(List<Company>));
        var stringBuilder = new StringBuilder();
        using (TextWriter writer = new StringWriter(stringBuilder))
        {
            serializer.Serialize(writer, companies);
        }
        var expectedXml = stringBuilder.ToString();

        // Act
        var result = await _controller.ExportXml();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualXml = Assert.IsType<string>(okResult.Value);
        Assert.Equal(expectedXml, actualXml);
    }

    // Добавление остальных тестов...

    [Fact]
    public async Task ImportXml_ReturnsOkResult_WhenValidXmlString()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        // Arrange
        var xmlString = "<Company><Name>Company1</Name></Company>";

        // Act
        var result = await _controller.ImportXml(xmlString);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
    }

    [Fact]
    public async Task MoveGroup_ReturnsOkResult_WhenGroupIsMoved()
    {
        // Arrange
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        var company = new Company { Name = "comp1" };
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        var dept = new Departament { Name = "dept1", CompanyId = company.Id };
        var dept2 = new Departament { Name = "dept2", CompanyId = company.Id };
        await _context.Departaments.AddRangeAsync(new List<Departament> { dept, dept2 });
        await _context.SaveChangesAsync();
        var group = new Group { DepartamentId = dept.Id, Name = "gr1" };
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();
        // Act
        var result = await _controller.MoveGroup(group.Id, dept2.Id);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateGroup_ReturnsOkResult_WhenGroupIsValid()
    {
        // Arrange
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        var company = new Company { Name = "comp1" };
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        var dept = new Departament { Name = "dept1", CompanyId = company.Id };
        var dept2 = new Departament { Name = "dept2", CompanyId = company.Id };
        await _context.Departaments.AddRangeAsync(new List<Departament> { dept, dept2 });
        await _context.SaveChangesAsync();
        var group = new Group { DepartamentId = dept.Id, Name = "gr1" };
        var group1 = new Group { DepartamentId = dept.Id, Name = "gr2" };

        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.CreateGroup(group1);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteGroup_ReturnsOkResult_WhenGroupExists()
    {
        // Arrange
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        var company = new Company { Name = "comp1" };
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        var dept = new Departament { Name = "dept1", CompanyId = company.Id };
        var dept2 = new Departament { Name = "dept2", CompanyId = company.Id };
        await _context.Departaments.AddRangeAsync(new List<Departament> { dept, dept2 });
        await _context.SaveChangesAsync();
        var group = new Group { DepartamentId = dept.Id, Name = "gr1" };
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteGroup(group);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteGroup_ReturnsNotFoundResult_WhenGroupDoesNotExist()
    {
        // Arrange
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        var company = new Company { Name = "comp1" };
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        var dept = new Departament { Name = "dept1", CompanyId = company.Id };
        var dept1 = new Departament { Name = "dept2", CompanyId = company.Id };
        await _context.Departaments.AddRangeAsync(new List<Departament> { dept, dept1 });
        await _context.SaveChangesAsync();
        var group = new Group { DepartamentId = dept.Id, Name = "gr1" };
        var group1 = new Group { DepartamentId = dept.Id, Name = "gr2" };

        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteGroup(group1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
