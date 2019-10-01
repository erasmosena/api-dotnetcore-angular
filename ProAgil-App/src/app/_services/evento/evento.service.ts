import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEvento } from 'src/app/_models';
;

@Injectable({
	providedIn: 'root'
})
export class EventoService {
	baseURL = 'http://localhost:5000/api/eventos';
	constructor(private http: HttpClient) { }

	getAll(): Observable<IEvento[]> {
		return this.http.get<IEvento[]>(`${this.baseURL}`);
	}

	getEventoByTema(tema: string): Observable<IEvento[]> {
		return this.http.get<IEvento[]>(`${this.baseURL}/GetByTema/${tema}`);
	}

	getById(id: number): Observable<IEvento> {
		return this.http.get<IEvento>(`${this.baseURL}/${id}`);
	}

	upload(file: File) {
		const fileToUpload = <File>file[0];
		const formData = new FormData();
		formData.append('file', fileToUpload, fileToUpload.name);
		return this.http.post(`${this.baseURL}/upload`, formData);
	}
	post(evento: IEvento) {
		return this.http.post(`${this.baseURL}`, evento);
	}

	put(idEvento: number, evento: IEvento) {
		return this.http.put(`${this.baseURL}/${idEvento}`, evento);
	}

	delete(idEvento: number) {
		return this.http.delete(`${this.baseURL}/${idEvento}`);
	}
}
