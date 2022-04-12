using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        //private static List<Filme> filmes = new List<Filme>();
        //private static int id = 1;
        private FilmeContext _context; 
        private IMapper _mapper; 
         
        public FilmeController(FilmeContext  context, IMapper mapper)//Injeção de dependencia: fizemos do AutoMapper e FilmeContext
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            //Criação de um objeto com um construtor implícito
            //Passamos para esse filme o valor das propriedades do nosso Dto e adicionamos ele ao nosso sistemas
            //Com esse mapeamento nós não precisamos PEDIR para o usuário passar, o Id nao será obrigatório, isso é o banco de dados que vai cuidar 
            //Vamos fazer isso usando o auto Mapper
            Filme filme = _mapper.Map<Filme>(filmeDto);

            //Console.WriteLine(filme.Titulo);
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperarFilmesPorId), new { Id = filme.Id }, filme);//Mostra além do status da requisição, aonde ele foi criado
        }

        [HttpGet]
        public IEnumerable<Filme> RecuperarFilmes()
        {
            return _context.Filmes;
        }

        //Coloco dentro de um parenteses o id como parametro e ele vai colocar dentro do parametro int id
        [HttpGet("{id}")]
        public IActionResult RecuperarFilmesPorId(int id)
        {
            //pacote auto mapper, que faz os mapeadmento entre diferentes classes e evita códigos manuais
            Filme filme =  _context.Filmes.FirstOrDefault(filme => filme.Id == id); //Se ele nao achar nenhum filme ele vai retornar nulo
            if(filme != null)
            {
                ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
               // nós colocamos uma informação extra, que é o DateTime e essa informacao nao é necessária p guardar no banco, entao colocamos em um DTO, é elegante
              

                return Ok(filmeDto); //chamada que vai informar que a resposta foi ok
            }

            return NotFound();

            //Colocamos o IAction Result porque o Ok() e o NotFound() são object result, sao um resultado da nossa ação, então nós retornamos com IAction Result
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            //recuperar o filme pelo id que queremos alterar e depois alteramos
            //qual é o filme que queremos alterar?
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme == null)
            {
                return NotFound();
            }
            //Diferente dos outros, aqui nao queremos converter um objeto para outro tipo, queremos converter 2 objetos entre si
            _mapper.Map(filmeDto, filme);//Quero pegar as informações do 'filmeDto' e jogar no 'filme'
            _context.SaveChanges();

            return NoContent();//Boa prática NoContent no put

        }

        [HttpDelete("{id}")]
        public IActionResult DeletaFilme(int id)
        {
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null)
            {
                return NotFound();
            }
            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }

       



    }
}
