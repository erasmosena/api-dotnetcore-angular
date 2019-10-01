import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { defineLocale, BsLocaleService, ptBrLocale } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
defineLocale('pt-br', ptBrLocale);

import { EventoService } from '../_services';
import { IEvento } from '../_models';



@Component({
	selector: 'app-eventos',
	templateUrl: './eventos.component.html',
	styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

	titulo = 'Eventos';
	novoRegistro = true;
	height = 50;
	width = 50;
	mostrarimagem = false;
	eventos: IEvento[] = [];
	evento: IEvento;
	eventosFiltrados: IEvento[] = [];
	_filtroLista = '';
	form: FormGroup;
	msgConfirmarExcluir = '';
	file: File;


	constructor(private service: EventoService
		, private fb: FormBuilder
		, private localeService: BsLocaleService
		, private toastrService: ToastrService
	) {
		this.localeService.use('pt-br');
	}

	get filtroLista() {
		return this._filtroLista;
	}
	set filtroLista(value) {
		this._filtroLista = value;
		this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
	}

	novoEvento(template: any) {
		this.novoRegistro = true;
		this.openModal(template);
	}

	editarEvento(template: any, idEvento: number) {
		this.novoRegistro = false;
		this.openModal(template);
		this.service.getById(idEvento).subscribe(
			(evento: IEvento) => {
				this.evento = evento;
				this.evento.imagemURL = '';
				this.form.patchValue(evento);
			}
		)

	}

	openModal(template: any) {
		this.form.reset();
		template.show();
	}

	uploadImagem() {

		this.evento.imagemURL = this.file[0].name;
		return this.service.upload(this.file);
	}

	salvarAlteracao(template) {
		if (this.form.valid) {

			if (this.novoRegistro) {
				this.evento = Object.assign({}, this.form.value);
				this.uploadImagem().toPromise().then(
					() => {
						this.service.post(this.evento).subscribe(

							(novoEvento: IEvento) => {
								this.getEventos();
								template.hide();
								this.toastrService.success('Evento Adicionado.', 'Executado com sucesso.');
							}, erro => {
								this.toastrService.error('Não foi possível adicionar o evento.', 'Erro ao adicionar dados');
							}
						);
					});
			} else {
				this.evento = Object.assign(this.evento, this.form.value);
				this.uploadImagem().toPromise().then(
					() => {
						this.service.put(this.evento.idEvento, this.evento).subscribe(
							(novoEvento: IEvento) => {
								this.getEventos();
								template.hide();
								this.toastrService.success('Evento Alterado.', 'Executado com sucesso.');
							}, erro => {
								this.toastrService.error('Não foi possível alterar o evento.', 'Erro ao alterar dados');
							}
						);
					}
				);
			}
		}
	}

	excluirEvento(evento: IEvento, template: any) {
		template.show();
		this.evento = evento;
		this.msgConfirmarExcluir = `Tem certeza que deseja excluir o Evento: ${this.evento.idEvento} - ${this.evento.tema} ?`;
	}

	delete(template: any) {
		this.service.delete(this.evento.idEvento).subscribe(
			() => {
				this.getEventos();
				template.hide();
				this.toastrService.success('Evento excluido.', 'Evento');
			},
			error => {
				this.toastrService.error('Não foi possível excluir o evento.', 'Erro ao excluir dados');
			}
		);
	}

	getEventos() {
		this.service.getAll().subscribe(
			(_eventos: IEvento[]) => {
				this.eventos = _eventos;
				this.eventosFiltrados = _eventos;
			},
			error => {
				this.toastrService.error('Não foi possível obter  os eventos.', 'Erro ao obter dados');
			}
		);
	}

	createForm() {
		this.form = this.fb.group({
			tema: ['',
				[
					Validators.required,
					Validators.minLength(4),
					Validators.maxLength(50)
				]
			],
			local: ['', Validators.required],
			dataEvento: ['', Validators.required],
			qtdPessoas: ['',
				[
					Validators.required,
					Validators.max(5000)
				]
			],
			imagemURL: ['', Validators.required],
			telefone: ['', Validators.required],
			email: ['',
				[
					Validators.required,
					Validators.email
				]
			],
		});

	}

	onFileChange(event) {
		const reader = new FileReader();
		if (event.target.files && event.target.files.length) {
			this.file = event.target.files;
		}
	}


	filtrarEventos(filtro): IEvento[] {
		filtro = filtro.toLocaleLowerCase();
		const lista = this.eventos.filter(it => {
			return it.tema.toLocaleLowerCase().indexOf(filtro) !== -1;
		});
		return lista;
	}

	get f() {
		return this.form.controls;
	}

	asdf() {
		return this.form.get('tema').errors;
	}

	ngOnInit() {
		this.getEventos();
		this.createForm();
	}

	alternarImagem() {
		this.mostrarimagem = !this.mostrarimagem;
	}


	//validações 
	isInvalid(campo: string) {
		return this.form.get(campo).errors &&
			this.form.get(campo).touched;
	}

	Invalid(campo: string, validation: string) {
		return this.form.get(campo).hasError(validation);
	}

	isInvalidRequired(campo: string) {
		return this.Invalid(campo, 'required');
	}
	isInvalidMax(campo: string) {
		return this.Invalid(campo, 'max');
	}
	isInvalidEmail(campo: string) {
		return this.Invalid(campo, 'email');
	}


}
