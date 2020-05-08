import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BuildNumberComponent } from './build-number.component';

describe('BuildNumberComponent', () => {
  let component: BuildNumberComponent;
  let fixture: ComponentFixture<BuildNumberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BuildNumberComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BuildNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
