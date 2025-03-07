import {Component, EventEmitter, Input, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {ApiConfig} from '../../config/api.config';
import {Card} from '../../models/card.model';

@Component({
  selector: 'app-update-card-form',
  standalone: false,

  templateUrl: './update-card-form.component.html',
  styleUrl: './update-card-form.component.css'
})
export class UpdateCardFormComponent {
	@Input() card!: Card;
	cardForm: FormGroup;
	selectedFile: { name: string, preview: string, isImage: boolean } | null = null;
	isHovering = false;
	loading = false;
	@Output() updateComplete: EventEmitter<void> = new EventEmitter();

	constructor(private fb: FormBuilder, private http: HttpClient) {
		this.cardForm = this.fb.group({
			cardName: ['', [Validators.required]],
			cardType: ['', [Validators.required]],
			cardFaction: ['', [Validators.required]],
			cardImage: [null, [Validators.required]]
		});
	}

	ngOnInit(): void {

		if (this.card) {
			this.cardForm.patchValue({
				cardName: this.card.cardName,
				cardType: this.card.type,
				cardFaction: this.card.faction,
			});

			if (this.card.imageUrl) {
				this.selectedFile = {
					name: 'Current Image',
					preview: this.card.imageUrl,
					isImage: true
				};

				this.cardForm.get('cardImage')?.clearValidators();
				this.cardForm.get('cardImage')?.updateValueAndValidity();
			}
		}
	}

	onDragOver(event: DragEvent) {
		event.preventDefault();
		this.isHovering = true;
	}

	onDragLeave(event: DragEvent) {
		event.preventDefault();
		this.isHovering = false;
	}

	onDrop(event: DragEvent) {
		event.preventDefault();
		this.isHovering = false;

		if (event.dataTransfer && event.dataTransfer.files.length > 0) {
			const file = event.dataTransfer.files[0];
			this.handleFile(file);
		}
	}

	onFileSelected(event: Event) {
		const input = event.target as HTMLInputElement;
		if (input.files && input.files.length > 0) {
			const file = input.files[0];
			this.handleFile(file);
		}
	}

	handleFile(file: File) {
		const reader = new FileReader();

		reader.onload = (e: any) => {
			const isImage = file.type.startsWith('image/');
			this.selectedFile = {
				name: file.name,
				preview: isImage ? e.target.result : '',
				isImage: isImage
			};

			this.cardForm.patchValue({
				cardImage: file
			});
		};

		reader.readAsDataURL(file);
	}

	onSubmit() {
		if (this.cardForm.valid) {
			this.loading = true;
			const formData = new FormData();
			formData.append('CardName', this.cardForm.get('cardName')?.value);
			formData.append('Type', this.cardForm.get('cardType')?.value);
			formData.append('Faction', this.cardForm.get('cardFaction')?.value);
			formData.append('Image', this.cardForm.get('cardImage')?.value);

			console.log('Form submitted:', this.cardForm.value);

			this.http.put(`${ApiConfig.apiUrl}/Card?cardID=${this.card.id}`, formData).subscribe(
				response => {
					this.cardForm.reset();
					this.selectedFile = null;
					this.loading = false;
					this.updateComplete.emit();
				}
			);
		} else {
			console.log('Form is invalid');
		}
	}
}
