
📦 DevIO.OrderProducts — Arquitetura Escalável e Resiliente

Este projeto é uma aplicação .NET moderna, modular e resiliente, com separação de responsabilidades em múltiplas camadas e microserviços, 
aplicando princípios de DDD, SOLID, CQRS, e integrando soluções como Redis, Kafka, Elastic Stack, Docker e Kubernetes.

📁 Estrutura da Solução

DevIO.OrderProducts/
├── DevIO.OrderProducts.WebApi/			→ API Principal (Produtos, Pedidos)
├── DevIO.Auth/							→ API de Autenticação OAuth2
├── DevIO.OrderProducts.Application		→ Camada Application (CQRS com MediatR)
├── DevIO.OrderProducts.Domain			→ Camada de Domínio (Entidades, Regras)
├── DevIO.OrderProducts.Infrastructure	→ Repositórios, EF, Redis, Kafka
├── DevIO.OrderProducts.Worker			→ Worker Kafka Consumer
└── k8s/								→ Manifests Kubernetes

🚀 Funcionalidades

✅ API RESTful com ASP.NET Core 8

✅ CRUD de Produtos, Pedidos e Itens de Pedido

✅ API de autenticação separada com JWT e OAuth 2.0

✅ CQRS com MediatR (Commands e Queries)

✅ Validações com FluentValidation

✅ Log estruturado com ELK (ElasticSearch, Logstash, Kibana)

✅ Cache com Redis

✅ Mensageria assíncrona com Apache Kafka

✅ Health Checks com UI

✅ Tratamento de erros e middleware de logging

✅ Docker + Kubernetes com Ingress, Secrets e ConfigMap


🛠️ Tecnologias e Ferramentas
Stack							Tecnologias
Back-end						ASP.NET Core 8, Entity Framework Core, MediatR
Banco de dados					SQL Server
Mensageria						Apache Kafka
Cache							Redis
Segurança						JWT, OAuth 2.0, Roles e Claims
Log								Serilog, Logstash, Elasticsearch, Kibana
Container						Docker, Docker Compose
Orquestração					Kubernetes (Deployments, Services, Ingress, Secrets, ConfigMaps)
Outros							xUnit (testes), AutoMapper, IOptions Pattern