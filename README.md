
# StubCriptografia

Este repositório contém um stub para criptografar e descriptografar arquivos executáveis (.exe) usando AES.

## Requisitos

- .NET SDK 7.0 ou superior

## Como compilar e executar

### Passo 1: Clonar o repositório

```bash
git clone https://github.com/iraci2024/StubCriptografia.git
cd StubCriptografia
```

### Passo 2: Compilar o projeto

```bash
dotnet build
```

### Passo 3: Criptografar um arquivo

```bash
dotnet bin/Debug/net7.0/StubProject.dll encrypt <caminho_do_arquivo_exe>
```

### Passo 4: Descriptografar um arquivo
```bash
dotnet bin/Debug/net7.0/StubProject.dll decrypt <caminho_do_arquivo_exe.enc>
```

### Passo 5: Converter um arquivo descriptografado para PDF
```bash
dotnet bin/Debug/net7.0/StubProject.dll convert_to_pdf <caminho_do_arquivo_txt>
```

## Exemplo

### Criptografar
```bash
dotnet bin/Debug/net7.0/StubProject.dll encrypt exemplo.exe
```

### Descriptografar
```bash
dotnet bin/Debug/net7.0/StubProject.dll decrypt exemplo.exe.enc
```

## Observações

- Certifique-se de que o .NET SDK está instalado e configurado corretamente no seu sistema.
- Use uma chave de criptografia segura para proteger seus arquivos.
