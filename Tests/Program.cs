using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;

IBaseRepository<T> GetRepository<T>(string type) where T : Model, new()
{
    return type switch
    {
        "dapper" => new DapperRepository<T>()
    };
}

Car car = new Car();

Console.WriteLine(car.Table());

/*
var repository = GetRepository<Pix>("dapper");

var pix = await repository.FindWith(p => p.Type);

Console.WriteLine(pix.FirstOrDefault()?.Type.Name);*/
