using Bogus;
using RazorPagesDB.Data;
using RazorPagesDB.Interfaces;
using RazorPagesDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RazorPagesDB.Seeders
{
    public class TareaSeeder : IDbInitializer
    {
        // Usamos 'override' para implementar el método abstracto de la clase base
        public override void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TareaDbContext>();
            context.Database.EnsureCreated(); // Asegura que la base de datos exista

            // Llama a la lógica de siembra de datos dentro de este método
            SeedTareaData(context);
        }
        private void SeedTareaData(TareaDbContext context)
        {
            // Ojo: Elimina los datos existentes para empezar de cero, si la tabla tiene id autoincremental el id continua aumentando 
            context.Tareas.RemoveRange(context.Tareas);
            context.SaveChanges(); //como se borraron los datos en el context, tiene que guardarse los datos en la tabla correspondiente.

            // Genera una lista de 50 tareas de prueba
            var tareaFaker = new Faker<Tarea>()
                //.RuleFor(t => t.Id, f => f.IndexFaker + 1) //Depende de nuestra tabla, si la tabla tiene un ID autoincremental esta línea se comenta, sino se descomenta para generar el ID
                .RuleFor(t => t.nombreTarea, f => f.Lorem.Sentence(3))
                .RuleFor(t => t.fechaVencimiento, f => f.Date.Future(1))
                .RuleFor(t => t.estado, f => f.PickRandom(new[] { "Pendiente", "En Curso", "Finalizado", "Cancelado" }))
                .RuleFor(t => t.IdUsuario, f => f.Random.Number(1, 10)); // Asume que tienes 10 usuarios de prueba, ojo, solo es id

            var tareas = tareaFaker.Generate(50);

            // Agrega las tareas generadas al contexto y guarda los cambios en la base de datos, si, en la base y esto puede ser verificable
            context.Tareas.AddRange(tareas);
            context.SaveChanges();
        }
    }
}