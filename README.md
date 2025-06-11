
# HealthMed

**HealthMed** é um sistema desenvolvido como parte do **Tech Challenge** da fase final do curso **Pos-Tech - Arquitetura de Sistemas .NET com Azure**. A atividade propõe um cenário realista em que os alunos atuam como consultores de software, sendo responsáveis por:

- Análise do problema  
- Definição da arquitetura  
- Implementação de um MVP funcional  
- Testes e documentação técnica  

O desafio integra conhecimentos adquiridos ao longo de todas as disciplinas da fase, promovendo uma aplicação prática e abrangente.

O projeto foi idealizado para atender às demandas da startup fictícia **Health&Med**, no desenvolvimento de uma plataforma própria para agendamento de consultas médicas. O objetivo é entregar um MVP com arquitetura moderna, baseada em microserviços, preparada para a nuvem e alinhada às boas práticas de segurança, escalabilidade e comunicação assíncrona entre serviços. A solução inclui autenticação via JWT e orquestração de serviços com Docker e Kubernetes.

## Arquitetura e Tecnologias

O sistema é composto por múltiplos microserviços organizados conforme o padrão Clean Architecture e utilizando tecnologias modernas como:

- .NET 8  
- Docker + Docker Compose para desenvolvimento local  
- Kubernetes para orquestração em ambientes de produção (arquivos disponíveis na pasta `/k8s`)  
- PostgreSQL como banco de dados  
- RabbitMQ para comunicação assíncrona (mensageria)  
- Prometheus + Grafana para monitoramento  
- MassTransit para abstração da mensageria  
- xUnit + FluentAssertions (testes)  
- Bogus (geração de dados fake)  
- JWT (Autenticação)  
- Clean Architecture (modularização por contexto)  

## Estrutura de Projeto

O projeto está dividido em múltiplos microserviços localizados no diretório `src/`, seguindo os domínios principais:

```
src/
├── HealthMed.Agenda.API
├── HealthMed.Agenda.Application
├── HealthMed.Agenda.Domain
├── HealthMed.Agenda.Infra.Consumer
├── HealthMed.Agenda.Infra.Consumer.IoC
├── HealthMed.Agenda.Infra.Data
├── HealthMed.Agenda.Infra.IoC
├── HealthMed.Auth.API
├── HealthMed.Auth.Application
├── HealthMed.Auth.Infra.Data
├── HealthMed.Auth.Infra.IoC
├── HealthMed.BuildingBlocks
├── HealthMed.Consultation.API
├── HealthMed.Consultation.Application
├── HealthMed.Consultation.Domain
├── HealthMed.Consultation.Infra.Consumer
├── HealthMed.Consultation.Infra.Consumer.IoC
├── HealthMed.Consultation.Infra.Data
├── HealthMed.Consultation.Infra.IoC
├── HealthMed.Doctor.Infra.Consumer
├── HealthMed.Doctor.Infra.Consumer.IoC
├── HealthMed.Domain
├── HealthMed.Gateway.API
├── HealthMed.Medico.API
├── HealthMed.Medico.Application
├── HealthMed.Medico.Domain
├── HealthMed.Medico.Infra.Data
├── HealthMed.Medico.Infra.IoC
├── HealthMed.Paciente.API
├── HealthMed.Paciente.Application
├── HealthMed.Paciente.Domain
├── HealthMed.Paciente.Infra.Data
├── HealthMed.Paciente.Infra.IoC
├── HealthMed.Patient.Infra.Consumer
├── HealthMed.Patient.Infra.Consumer.IoC
├── HealthMed.Agenda.API/Controllers
├── HealthMed.Agenda.Application/Consumers
├── HealthMed.Agenda.Application/Events
├── HealthMed.Agenda.Application/Interfaces
├── HealthMed.Agenda.Application/UseCases
├── HealthMed.Agenda.Application/ViewModels
├── HealthMed.Agenda.Domain/Core
├── HealthMed.Agenda.Domain/Entities
├── HealthMed.Agenda.Domain/Interfaces
├── HealthMed.Agenda.Infra.Data/Context
├── HealthMed.Agenda.Infra.Data/Repositories
├── HealthMed.Auth.API/Controllers
├── HealthMed.Auth.Application/Interfaces
├── HealthMed.Auth.Application/UseCases
├── HealthMed.Auth.Application/ViewModels
├── HealthMed.Auth.Infra.Data/Context
├── HealthMed.Auth.Infra.Data/Repositories
├── HealthMed.Consultation.API/Controllers
├── HealthMed.Consultation.Application/Consumers
├── HealthMed.Consultation.Application/Events
├── HealthMed.Consultation.Application/Interfaces
├── HealthMed.Consultation.Application/Publisher
├── HealthMed.Consultation.Application/UseCases
├── HealthMed.Consultation.Application/ViewModels
├── HealthMed.Consultation.Domain/Core
├── HealthMed.Consultation.Domain/Entities
├── HealthMed.Consultation.Domain/Interfaces
├── HealthMed.Consultation.Domain/Validations
├── HealthMed.Consultation.Infra.Data/Context
├── HealthMed.Consultation.Infra.Data/Repositories
├── HealthMed.Domain/Core
├── HealthMed.Domain/Entities
├── HealthMed.Domain/Interfaces
├── HealthMed.Gateway.API/Controllers
├── HealthMed.Medico.API/Controllers
├── HealthMed.Medico.Application/Consumers
├── HealthMed.Medico.Application/Events
├── HealthMed.Medico.Application/Interfaces
├── HealthMed.Medico.Application/Publisher
├── HealthMed.Medico.Application/UseCases
├── HealthMed.Medico.Application/ViewModels
├── HealthMed.Medico.Domain/Core
├── HealthMed.Medico.Domain/Entities
├── HealthMed.Medico.Domain/Interfaces
├── HealthMed.Medico.Domain/Validations
├── HealthMed.Medico.Infra.Data/Context
├── HealthMed.Medico.Infra.Data/Repositories
├── HealthMed.Paciente.API/Controllers
├── HealthMed.Paciente.Application/Consumers
├── HealthMed.Paciente.Application/Events
├── HealthMed.Paciente.Application/Interfaces
├── HealthMed.Paciente.Application/UseCases
├── HealthMed.Paciente.Application/ViewModels
├── HealthMed.Paciente.Domain/Core
├── HealthMed.Paciente.Domain/Entities
├── HealthMed.Paciente.Domain/Interfaces
├── HealthMed.Paciente.Domain/Validations
├── HealthMed.Paciente.Infra.Data/Context
├── HealthMed.Paciente.Infra.Data/Repositories
```

Cada serviço segue a separação por camadas:

- **Domain:** Entidades e validações  
- **Application:** Use cases, interfaces, publishers  
- **Infrastructure:** Acesso a dados (EF Core), integração com fila, banco, etc.  
- **API:** Controllers e endpoints  

## Como Executar

### Requisitos

- Docker e Docker Compose  
- .NET 8 SDK  
- (Opcional) Minikube, Docker Desktop com Kubernetes, ou AKS para execução via Kubernetes  

### Ambiente de Desenvolvimento Local (Docker Compose)

```bash
docker-compose up --build
```

A aplicação será iniciada com todos os serviços disponíveis em seus respectivos containers.

### Ambiente com Kubernetes

Os arquivos YAML para deploy estão na pasta `/k8s`. Para executar:

```bash
kubectl apply -f ./k8s
```

Certifique-se de configurar os `Secrets`, `ConfigMaps` e `Ingress` conforme seu ambiente.

Isso iniciará:

- APIs dos microserviços  
- RabbitMQ (acessível em http://localhost:15672)  
- PostgreSQL  
- Prometheus (http://localhost:9090)  
- Grafana (http://localhost:3000)  

A senha padrão do Grafana é: `admin / admin`.

## Monitoramento

- **Prometheus** coleta métricas de cada microserviço.  
- **Grafana** exibe painéis e gráficos em tempo real.  

## Contribuidores

- Wagner Cristiano Parmezão 
