using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Data;
using PrimeiraApi.Models;
using PrimeiraApi.Models.Requests;

namespace PrimeiraApi.Routes
{
    public static class PersonRoutes
    {
        public static void MapPersonRoutes( this WebApplication app )
        {
            var RoutePeople = app.MapGroup( prefix: "people" );

            RoutePeople.MapGet( "", handler: async (AppDbContext context) =>
            {
                var people = await context.People
                .Where(person => person.Active)
                .Select(person => new PersonDto(person.Id, person.Name, person.Job))
                .ToListAsync();

                return Results.Ok( people );
            } );

            RoutePeople.MapGet( "/{name}" , async ( string name , AppDbContext context ) =>
            {
                var person = await context.People
                .SingleOrDefaultAsync( person => person.Name.StartsWith(name) );

                if (person == null)
                    return Results.NotFound();

                return Results.Ok( new PersonDto( person.Id , person.Name , person.Job ) );
            } );

            RoutePeople.MapPost( "" , async ( AddPersonRequest request , AppDbContext context ) =>
            {
                var alreadyExists = await context.People.AnyAsync( person => person.Name == request.Name );
                
                if (alreadyExists)
                {
                    return Results.Conflict();
                }

                var person = new Person( request.Name , request.Job );
                await context.People.AddAsync( person );
                await context.SaveChangesAsync();

                return Results.Ok(new PersonDto( person.Id , person.Name , person.Job ) );
            } );

            RoutePeople.MapPut( "{id}" , async (Guid id, UpdatePersonNameRequest req, AppDbContext context) =>
            {
                var person = await context.People
                .SingleOrDefaultAsync( person => person.Id == id );

                if (person == null)
                    return Results.NotFound();

                person.UpdateName( req.Name );

                await context.SaveChangesAsync();

                return Results.Ok( new PersonDto( person.Id , person.Name , person.Job ) );
            } );

            RoutePeople.MapPut( "{id}/job" , async ( Guid id , UpdatePersonJobRequest req , AppDbContext context ) =>
            {
                var person = await context.People
                .SingleOrDefaultAsync( person => person.Id == id );

                if (person == null)
                    return Results.NotFound();

                person.UpdateJob( req.Job );

                await context.SaveChangesAsync();

                return Results.Ok( new PersonDto( person.Id , person.Name , person.Job ) );
            } );

            RoutePeople.MapDelete( "{id}" , async ( Guid id , AppDbContext context ) =>
            {
                var person = await context.People
                .SingleOrDefaultAsync( person => person.Id == id );

                if (person == null)
                    return Results.NotFound();

                person.Disable();

                await context.SaveChangesAsync();

                return Results.Ok();
            } );
        }
    }
}
