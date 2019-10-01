import { IEvento } from './IEvento';
import { IPalestrante } from './IPalestrante';

export interface IPalestranteEvento {
	idPalestrante: number;
	palestrante: IPalestrante;
	idEvento: number;
	evento: IEvento;
}