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
/*
var repository = GetRepository<Pix>("dapper");

var pix = await repository.FindWith(p => p.Type);

Console.WriteLine(pix.FirstOrDefault()?.Type.Name);*/

var repository = GetRepository<Pix>("dapper");

var pixes = (await repository.FindWith(1, p => p.Type));

Console.WriteLine();