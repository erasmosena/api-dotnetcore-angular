import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { TooltipModule, BsDropdownModule, ModalModule, BsDatepickerModule } from 'ngx-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { ToastrModule } from 'ngx-toastr';

import { EventoService } from './_services';

import { DatetimeFormatPipePipe } from './_helps/DatetimeFormatPipe.pipe';

import { AppComponent } from './app.component';
import { EventosComponent } from './eventos/eventos.component';
import { NavComponent } from './nav/nav.component';
import { DebugComponent } from './_components/debug/debug.component';
import { PalestranteComponent } from './palestrante/palestrante.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContatoComponent } from './contato/contato.component';
import { BarraTituloComponent } from './_shared/barra-titulo/barra-titulo.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';

@NgModule({
	declarations: [
		AppComponent,
		NavComponent,
		BarraTituloComponent,
		ContatoComponent,
		DashboardComponent,
		EventosComponent,
		PalestranteComponent,
		DatetimeFormatPipePipe,
		DebugComponent,
		UserComponent,
		LoginComponent,
		RegistrationComponent
	],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		ToastrModule.forRoot(),
		TooltipModule.forRoot(),
		BsDropdownModule.forRoot(),
		ModalModule.forRoot(),
		BsDatepickerModule.forRoot(),
		ReactiveFormsModule,
		HttpClientModule,
		AppRoutingModule
	],
	providers: [
		EventoService
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
