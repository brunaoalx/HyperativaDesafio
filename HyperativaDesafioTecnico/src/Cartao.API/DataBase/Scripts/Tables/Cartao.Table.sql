CREATE TABLE cartao (
    id            INTEGER    PRIMARY KEY,
    numeroHash    TEXT (100),
    numeroMascara TEXT (100),
    numeracaoLote TEXT (100),
    dataCadastro  TEXT (100),
    lote          INTEGER    CONSTRAINT fk_cartao_lote REFERENCES lote (id)
);