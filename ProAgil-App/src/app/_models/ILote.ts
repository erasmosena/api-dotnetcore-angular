import { IEvento } from './IEvento';

export interface ILote {
	idLote: number;
	nome: string;
	preco: number;
	dataInicio?: Date;
	dataFim?: Date;
	quantidade: number;
	idEvento: number;
	evento: IEvento;
}
