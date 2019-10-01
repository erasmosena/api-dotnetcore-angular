import { IRedeSocial } from './IRedeSocial';
import { IPalestranteEvento } from './IPalestranteEvento';

export interface IPalestrante {
	PalestranteId: number;
	Nome: string;
	MiniCurriculo: string;
	ImagemUrl: string;
	Telefone: string;
	Email: string;
	RedeSocials: IRedeSocial[];
	PalestranteEventos: IPalestranteEvento[];
}
