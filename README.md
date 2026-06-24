# AppRpgEtec

Sistema Mobile desenvolvido em .NET MAUI para gerenciamento de personagens de RPG, disputas entre combatentes, utilização de armas e habilidades especiais, além de recursos de geolocalização, mapas e integração com APIs REST.

## Objetivo

O projeto AppRpgEtec foi desenvolvido durante a disciplina de Programação de Aplicativos Mobile II com o objetivo de aplicar conceitos de desenvolvimento mobile utilizando .NET MAUI, consumo de APIs REST, arquitetura MVVM, autenticação de usuários, geolocalização, mapas, armazenamento em nuvem e disputas entre personagens de RPG.

---

# Funcionalidades

## Usuários

* Cadastro de usuários
* Autenticação/Login
* Armazenamento de Token JWT
* Atualização de geolocalização
* Consulta de usuários
* Visualização de localização em mapa

---

## Personagens

* Cadastro de personagens
* Atualização de personagens
* Exclusão de personagens
* Consulta de personagens
* Pesquisa por nome aproximado
* Validação de campos obrigatórios
* Indicador visual de pontos de vida
* Restauração de pontos de vida
* Reset de ranking individual
* Reset geral de ranking

---

## Disputas

### Disputa com Armas

Permite realizar batalhas entre dois personagens utilizando as armas cadastradas para cada combatente.

### Disputa com Habilidades

Permite selecionar uma habilidade específica do personagem atacante para realizar o combate.

### Disputa Geral

Executa uma disputa envolvendo todos os personagens cadastrados no sistema, retornando os resultados da batalha.

---

## Geolocalização

* Captura da localização atual do dispositivo
* Atualização da localização na API
* Exibição dos usuários em mapa
* Integração com Google Maps

---

## Recursos Visuais

* Menu Flyout
* Navegação utilizando Shell
* Indicadores visuais de vida dos personagens
* Interface responsiva para dispositivos móveis

---

# Tecnologias Utilizadas

## Frontend Mobile

* .NET MAUI
* XAML
* C#

## Arquitetura

* MVVM (Model View ViewModel)

## Consumo de API

* REST API
* JSON

## Recursos Mobile

* Geolocalização
* Google Maps
* Armazenamento Local (Preferences)

## Armazenamento em Nuvem

* Azure Blob Storage

---

# Estrutura do Projeto

```text
AppRpgEtec
│
├── Models
│   ├── Usuario
│   ├── Personagem
│   ├── Arma
│   ├── Habilidade
│   ├── PersonagemHabilidade
│   └── Disputa
│
├── Services
│   ├── Usuarios
│   ├── Personagens
│   ├── Armas
│   ├── Disputas
│   └── PersonagemHabilidades
│
├── ViewModels
│   ├── Usuarios
│   ├── Personagens
│   ├── Armas
│   └── Disputas
│
├── Views
│   ├── Usuarios
│   ├── Personagens
│   ├── Armas
│   └── Disputas
│
├── Converters
│   └── PontosVidaConverter
│
└── Platforms
```

---

# Padrões Utilizados

## MVVM

O projeto utiliza o padrão MVVM para separação de responsabilidades entre:

* Model
* View
* ViewModel

---

## Commands

As ações da interface são executadas através de ICommand.

Exemplos:

* SalvarCommand
* CancelarCommand
* DisputaComArmaCommand
* DisputaComHabilidadeCommand
* DisputaGeralCommand

---

# Recursos Implementados

## Busca de Personagens

Pesquisa dinâmica utilizando SearchBar.

## Conversor de Pontos de Vida

As cores indicam o estado do personagem:

| Vida    | Cor         |
| ------- | ----------- |
| 100     | Verde       |
| 75 a 99 | Verde Claro |
| 25 a 74 | Amarelo     |
| 1 a 24  | Laranja     |
| 0       | Vermelho    |

---

## API Consumida

Endpoints utilizados:

### Usuários

```http
GET /Usuarios
POST /Usuarios
PUT /Usuarios
```

### Personagens

```http
GET /Personagens/GetAll
GET /Personagens/{id}
POST /Personagens
PUT /Personagens
DELETE /Personagens/{id}
```

### Disputas

```http
POST /Disputas/Arma
POST /Disputas/Habilidade
POST /Disputas/DisputaEmGrupo
```

---

# Como Executar

## Requisitos

* Visual Studio 2022
* .NET 8 ou superior
* Android SDK
* Emulador Android ou dispositivo físico

## Instalação

1. Clone o repositório

```bash
git clone <repositorio>
```

2. Abra a solução

```text
AppRpgEtec.sln
```

3. Restaure os pacotes NuGet

4. Compile o projeto

5. Execute em um dispositivo Android

---

# Aprendizados

Durante o desenvolvimento deste projeto foram aplicados conhecimentos de:

* Programação Orientada a Objetos
* Desenvolvimento Mobile
* MVVM
* Consumo de APIs REST
* Geolocalização
* Google Maps
* Azure Blob Storage
* Autenticação
* Navegação com Shell
* Manipulação de Dados
* XAML

---

# Integrantes

| Nome               | RM     |
| ------------------ | ------ |
| Lucas Soler Chanhi | 251400 |
| Icaro Dias Camargo | 190585 |

---

# Instituição

ETEC Professor Horácio Augusto da Silveira

Curso Técnico em Desenvolvimento de Sistemas

Disciplina: Programação de Aplicativos Mobile II

Professor: Luiz Fernando Souza

Ano: 2026
