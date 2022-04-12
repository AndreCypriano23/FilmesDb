using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Profiles
{
    public class FilmeProfile : Profile
    {
        //Vamos criar os perfis para o AutoMapper
        public FilmeProfile()
        {
            CreateMap<CreateFilmeDto, Filme>();//De que para que
            CreateMap<Filme, ReadFilmeDto>();
            CreateMap<UpdateFilmeDto, Filme>();
        }
    }
}
