import { Component } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { DataViewModule } from 'primeng/dataview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';

interface product {
  name: string;
  price: number;
  image: string;
  category: string;
  rating: number;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  products: product[] = [
    { name: "Test 1", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
    { name: "Test 2", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
    { name: "Test 3", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
    { name: "Test 4", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
    { name: "Test 5", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
    { name: "Test 6", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
    { name: "Test 7", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
    { name: "Test 8", price: 12, image: "bamboo-watch.jpg", rating: 5, category: "Test" },
  ]
  responsiveOptions = [
  {
    breakpoint: '1199px',
    numVisible: 1,
    numScroll: 1
  },
  {
    breakpoint: '991px',
    numVisible: 2,
    numScroll: 1
  },
  {
    breakpoint: '767px',
    numVisible: 1,
    numScroll: 1
  }
];
}
