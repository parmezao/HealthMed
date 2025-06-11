using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.UseCases;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;
using MassTransit;
using Moq;
using System.Text.Json;
using FluentAssertions;
namespace HealthMed.Doctor.Application.Tests;


    public class MedicoUseCasesTests
    {
        [Fact(DisplayName = "Insert: Deve publicar evento de insert com dados corretos e retornar mensagem de sucesso")]
        public async Task Insert_Execute_DevePublicarEventoERetornarMensagem()
        {
            var mockPublisher = new Mock<IMedicoPublisher>();
            var useCase = new InsertMedicoUseCase(mockPublisher.Object);

            var request = new InsertMedicoRequest(
            "Dr. Teste",
                "Cardiologia",
                "CRM123",
                 new List<HorarioDto> { new() { DataHora = DateTime.Now, Ocupado = false } }
            );

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Message.Should().Be("Cadastro em processamento.");
            mockPublisher.Verify(p => p.PublishInsertMedicoAsync(It.IsAny<InsertMedicoEvent>()), Times.Once);
        }

        [Fact(DisplayName = "Insert: Deve lançar exceção se nome for inválido")]
        public async Task Insert_Execute_DeveLancarExcecao_SeNomeInvalido()
        {
            var mockPublisher = new Mock<IMedicoPublisher>();
            var useCase = new InsertMedicoUseCase(mockPublisher.Object);

            var request = new InsertMedicoRequest(  "", "Clínico",  "CRM001", new List<HorarioDto>() );

            var act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("O nome é obrigatório.");
        }

        [Fact(DisplayName = "Update: Deve atualizar e publicar evento")]
        public async Task Update_Execute_DeveAtualizarEPublicarEvento()
        {
            var mockRepo = new Mock<IMedicoRepository>();
            var mockPub = new Mock<IMedicoPublisher>();
            var useCase = new UpdateMedicoUseCase(mockPub.Object, mockRepo.Object);

            mockRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new Medico("Dr. Antigo", "Geral", "CRM001", new()));

            var request = new UpdateMedicoRequest(
           
                1,
                "Dr. Atualizado",
                "Cardiologia",
                "CRM999",
                 new List<HorarioDto>()
            );

            var result = await useCase.Execute(request);

            result.Message.Should().Be("Atualização em processamento.");
            mockPub.Verify(p => p.PublishUpdateMedicoAsync(It.IsAny<UpdateMedicoEvent>()), Times.Once);
        }

        [Fact(DisplayName = "Update: Deve lançar exceção se médico não for encontrado")]
        public async Task Update_Execute_DeveLancarSeMedicoNaoEncontrado()
        {
            var mockRepo = new Mock<IMedicoRepository>();
            var mockPub = new Mock<IMedicoPublisher>();
            var useCase = new UpdateMedicoUseCase(mockPub.Object, mockRepo.Object);

            mockRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Medico)null!);

            var request = new UpdateMedicoRequest (99, "Dr. Falho", "CRM000", "Geral" ,new List<HorarioDto>() );

            var act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Não foi possível localizar o cadastro do médico informado.");
        }

        [Fact(DisplayName = "Delete: Deve publicar evento de exclusão")]
        public async Task Delete_Execute_DevePublicarEvento()
        {
            var mockRepo = new Mock<IMedicoRepository>();
            var mockPub = new Mock<IMedicoPublisher>();
            var useCase = new DeleteMedicoUseCase(mockPub.Object, mockRepo.Object);

            mockRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new Medico("Dr. Excluir", "Geral", "CRM001", new()));

            var result = await useCase.Delete(1);

            result.Message.Should().Be("Exclusão em processamento.");
            mockPub.Verify(p => p.PublishDeleteMedicotAsync(It.IsAny<DeleteMedicoEvent>()), Times.Once);
        }

        [Fact(DisplayName = "Delete: Deve lançar exceção se Médico não encontrado!")]
        public async Task Delete_Execute_DeveLancarSeMedicoNaoEncontrado()
        {
            var mockRepo = new Mock<IMedicoRepository>();
            var mockPub = new Mock<IMedicoPublisher>();
            var useCase = new DeleteMedicoUseCase(mockPub.Object, mockRepo.Object);

            mockRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Medico)null!);

            var act = async () => await useCase.Delete(123);

            await act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Médico não encontrado");
        }

        [Fact(DisplayName = "GetAll: Deve retornar lista de médicos")]
        public async Task GetAll_DeveRetornarLista()
        {
            var mockRepo = new Mock<IMedicoRepository>();
            mockRepo.Setup(r => r.GetAll("")).ReturnsAsync(new List<Medico>
        {
            new("Dr. A", "Cardio", "CRM01", new())
        });

            var useCase = new GetMedicosUseCase(mockRepo.Object);

            var result = await useCase.GetAll("");

            result.Should().HaveCount(1);
        }

        [Fact(DisplayName = "GetByCRM: Deve retornar médico pelo CRM")]
        public async Task GetByCRM_DeveRetornarMedico()
        {
            var mockRepo = new Mock<IMedicoRepository>();
            mockRepo.Setup(r => r.ObterPorCrmAsync("CRM123")).ReturnsAsync(new Medico("Dr. CRM", "Cardio", "CRM123", new()));

            var useCase = new GetMedicosUseCase(mockRepo.Object);
            var result = await useCase.ObterPorCrmAsync("CRM123");

            result.CRM.Should().Be("CRM123");
        }

        [Fact(DisplayName = "GetByCRM: Deve lançar exceção se não encontrado")]
        public async Task GetByCRM_DeveLancarSeNaoEncontrado()
        {
            var mockRepo = new Mock<IMedicoRepository>();
            mockRepo.Setup(r => r.ObterPorCrmAsync("CRM999")).ReturnsAsync((Medico)null!);

            var useCase = new GetMedicosUseCase(mockRepo.Object);

            var act = async () => await useCase.ObterPorCrmAsync("CRM999");

            await act.Should().ThrowAsync<ApplicationException>().WithMessage("Médico não encontrado!");
        }

        [Fact(DisplayName = "GetEspecialidade: Deve retornar por nome")]
        public async Task GetEspecialidade_DeveRetornarPorNome()
        {
            var mockRepo = new Mock<IEspecialidadeRepository>();
            mockRepo.Setup(r => r.GetByNome("Cardio"))
                .ReturnsAsync(new Especialidade(  "Cardio",   "Clínico" ));

            var useCase = new GetEspecialidadeUseCase(mockRepo.Object);
            var result = await useCase.GetByNome("Cardio");

            result!.Nome.Should().Be("Cardio");
        }

        [Fact(DisplayName = "GetEspecialidade: Deve lançar se nome não encontrado")]
        public async Task GetEspecialidade_DeveLancarSeNomeNaoEncontrado()
        {
            var mockRepo = new Mock<IEspecialidadeRepository>();
            mockRepo.Setup(r => r.GetByNome("Inexistente"))
                .ReturnsAsync((Especialidade)null!);

            var useCase = new GetEspecialidadeUseCase(mockRepo.Object);
            var act = async () => await useCase.GetByNome("Inexistente");

            await act.Should().ThrowAsync<ApplicationException>().WithMessage("Especialidade não encontrado!");
        }
    }