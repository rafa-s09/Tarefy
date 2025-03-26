-- Cria o banco de dados
CREATE DATABASE tarefy_db;

-- Seta a tabela e cria a mesma
USE tarefy_db;

CREATE TABLE tb_tarefas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descricao VARCHAR(255) NOT NULL,
    Detalhes VARCHAR(MAX),
    DataCriacao DATETIME DEFAULT GETDATE(),
    DataLimite DATETIME, 
    Prioridade INT DEFAULT 0,
    Status INT DEFAULT 0, 
);


-- Inserir
CREATE PROCEDURE sp_insert
    @Descricao  VARCHAR(255),
    @Detalhes   VARCHAR(MAX) = NULL,
    @DataLimite DATETIME = NULL,
    @Prioridade INT = 0,
    @Status     INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tb_tarefas (Descricao, Detalhes, DataLimite, Prioridade, Status)
    VALUES (@Descricao, @Detalhes, @DataLimite, @Prioridade, @Status);
END;


-- Atualizar
CREATE PROCEDURE sp_update
    @Id         INT,
    @Descricao  VARCHAR(255),
    @Detalhes   VARCHAR(MAX) = NULL,
    @DataLimite DATETIME = NULL,
    @Prioridade INT = 0,
    @Status     INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tb_tarefas SET Descricao = @Descricao, Detalhes = @Detalhes, DataLimite = @DataLimite, Prioridade = @Prioridade,Status = @Status WHERE Id = @Id;
END;


-- Deletar
CREATE PROCEDURE sp_delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tb_tarefas WHERE Id = @Id;
END;

-- Selecionar
CREATE PROCEDURE sp_select AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Descricao, Detalhes, DataCriacao, DataLimite, Prioridade, Status FROM tb_tarefas
END;

-- Selecionar pelo Id
CREATE PROCEDURE sp_select_by_id
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
    SELECT Id, Descricao, Detalhes, DataCriacao, DataLimite, Prioridade, Status
    FROM tb_tarefas WHERE Id = @Id;
END