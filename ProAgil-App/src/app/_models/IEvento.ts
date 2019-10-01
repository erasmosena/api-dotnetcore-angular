import { ILote } from './ILote';
import { IRedeSocial } from './IRedeSocial';
import { IPalestranteEvento } from './IPalestranteEvento';

export interface IEvento {
	idEvento: number;
	local: string;
	dataEvento: Date;
	tema: string;
	qtdPessoas: number;
	imagemURL: string;
	telefone: number;
	email: string;
	lotes: ILote[];
	redeSocials: IRedeSocial [] ;
	palestranteEventos: IPalestranteEvento[];

}

