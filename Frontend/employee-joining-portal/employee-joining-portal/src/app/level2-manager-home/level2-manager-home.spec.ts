import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Level2ManagerHome } from './level2-manager-home';

describe('Level2ManagerHome', () => {
  let component: Level2ManagerHome;
  let fixture: ComponentFixture<Level2ManagerHome>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Level2ManagerHome],
    }).compileComponents();

    fixture = TestBed.createComponent(Level2ManagerHome);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
