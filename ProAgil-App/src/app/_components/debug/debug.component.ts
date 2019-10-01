import { FormGroup } from '@angular/forms';
import { Component, OnInit, Input } from '@angular/core';

@Component({
	selector: 'app-debug',
	templateUrl: './debug.component.html',
	styleUrls: ['./debug.component.css']
})
export class DebugComponent implements OnInit {

	habilitar = false;
	@Input() form: FormGroup;
	@Input() name: String;
	constructor() {}

	ngOnInit() {
	}

}
