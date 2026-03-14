using Tarefas.API.Data;
using Tarefas.API.Services.ProdutoServices;
using TarefasBlazor.Shared.INFRA.ServicesComum.GeradorDeIDsService;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

public class GravarProdutoService : RetornoPadraoService
{
    private readonly ValidarProdutoService _validarProdutoService;
    private readonly ProdutoRepository<DbContextTarefas> _produtoRepository;
    private readonly IWebHostEnvironment _webHostEnvironment; // Para salvar arquivos
    private readonly string _imagensBasePath = "images/products"; // Subpasta para imagens

    public GravarProdutoService(
        ValidarProdutoService validarProdutoService,
        ProdutoRepository<DbContextTarefas> produtoRepository,
        IWebHostEnvironment webHostEnvironment)
    {
        _validarProdutoService = validarProdutoService;
        _produtoRepository = produtoRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task GravarProduto(ProdutoRequestDto dto, List<IFormFile>? novasImagens)
    {
        var produto = await _produtoRepository.ObterPorIdComRelacionamentosAsync(dto.Id);
        var ehAtualizacao = produto != null;
        
        var dtoProdutoTemErros = await _validarProdutoService.ValidarProduto(dto, ehAtualizacao);
        
        if (dtoProdutoTemErros)
        {
            Mensagens.AddRange(_validarProdutoService.Mensagens);
            return;
        }


        if (ehAtualizacao)
        {
            MapearDtoParaProduto(dto, produto!);
            await AtualizarImagens(dto, produto!, novasImagens);
            AtualizarEspecificacoes(dto, produto!);

            _produtoRepository.DbSet.Update(produto!);
        }
        else
        {
            produto = new Produto { Id = CriarIDService.CriarNovoID() };
            MapearDtoParaProduto(dto, produto);
            produto.Imagens = await SalvarNovasImagens(novasImagens, produto.Id);
            produto.Especificacoes = MapearEspecificacoesDtoParaEntidade(dto.Especificacoes, produto.Id);

            await _produtoRepository.DbSet.AddAsync(produto);
        }

        await _produtoRepository.DbContext.SaveChangesAsync();
    }

    private void MapearDtoParaProduto(ProdutoRequestDto dto, Produto produto)
    {
        produto.Nome = dto.Nome!;
        produto.Descricao = dto.Descricao!;
        produto.CodigoUnico = dto.CodigoUnico!;
        produto.Preco = dto.Preco;
        produto.PrecoPromocional = dto.PrecoPromocional;
        produto.EstaEmPromocao = dto.EstaEmPromocao;
        produto.TemFreteGratis = dto.TemFreteGratis;
        produto.QuantidadeEstoque = dto.QuantidadeEstoque!.Value;
        produto.EstaAtivo = dto.EstaAtivo;
        produto.MarcaId = dto.MarcaId;
        produto.CategoriaId = dto.CategoriaId;
        produto.FornecedorId = dto.FornecedorId;
    }

    private async Task AtualizarImagens(ProdutoRequestDto dto, Produto produto, List<IFormFile>? novasImagens)
    {
        var urlsImagensDto = dto.Imagens?.Select(i => i.UrlImagem).Where(url => !string.IsNullOrEmpty(url)).ToList() ?? new List<string>();
        var imagensAtuais = produto.Imagens.ToList();

        var imagensParaRemover = imagensAtuais.Where(imgDb => !urlsImagensDto.Contains(imgDb.UrlImagem)).ToList();
        foreach (var imagemRemover in imagensParaRemover)
        {
       
            RemoverArquivoImagem(imagemRemover.UrlImagem);
          
            produto.Imagens.Remove(imagemRemover);
        }
     
        var imagensSalvas = await SalvarNovasImagens(novasImagens, produto.Id);
        foreach (var novaImagem in imagensSalvas)
        {
            produto.Imagens.Add(novaImagem);
        }
    }

    private async Task<List<ProdutoImagem>> SalvarNovasImagens(List<IFormFile>? novasImagens, Guid produtoId)
    {
        var listaEntidadesImagem = new List<ProdutoImagem>();
        if (novasImagens == null || !novasImagens.Any())
        {
            return listaEntidadesImagem;
        }

        string pastaDestino = Path.Combine(_webHostEnvironment.WebRootPath, _imagensBasePath);
        Directory.CreateDirectory(pastaDestino);

        int ordem = 0;
        foreach (var formFile in novasImagens)
        {
            if (formFile.Length > 0)
            {

                string extensao = Path.GetExtension(formFile.FileName);
                string nomeUnico = $"{Guid.NewGuid()}{extensao}";
                string caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

               
                string urlRelativa = Path.Combine("/", _imagensBasePath, nomeUnico).Replace("\\", "/");

                listaEntidadesImagem.Add(new ProdutoImagem
                {
                    Id = CriarIDService.CriarNovoID(),
                    ProdutoId = produtoId,
                    UrlImagem = urlRelativa,
                    TextoAlternativo = formFile.FileName,
                    Ordem = ordem++ 
                });
            }
        }
        return listaEntidadesImagem;
    }

    private void RemoverArquivoImagem(string urlRelativa)
    {
        if (string.IsNullOrEmpty(urlRelativa)) return;

        // Converte a URL relativa para o caminho físico
        string caminhoFisico = Path.Combine(_webHostEnvironment.WebRootPath, urlRelativa.TrimStart('/', '\\').Replace("/", "\\"));
        if (File.Exists(caminhoFisico))
        {
            File.Delete(caminhoFisico);
        }
    }

    private void AtualizarEspecificacoes(ProdutoRequestDto dto, Produto produto)
    {
        produto.Especificacoes.Clear();
        produto.Especificacoes = MapearEspecificacoesDtoParaEntidade(dto.Especificacoes, produto.Id);
    }

    private List<ProdutoEspecificacao> MapearEspecificacoesDtoParaEntidade(List<ProdutoEspecificacaoDto> dtos, Guid produtoId)
    {
        return dtos.Select(itemDto => new ProdutoEspecificacao
        {
            Id = CriarIDService.CriarNovoID(),
            ProdutoId = produtoId,
            Chave = itemDto.Chave,
            Valor = itemDto.Valor
        }).ToList();
    }
}