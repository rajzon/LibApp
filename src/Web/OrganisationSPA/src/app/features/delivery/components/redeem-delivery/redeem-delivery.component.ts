import {AfterViewInit, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {DeliveryFacade} from "../../delivery.facade";
import {NgxSpinnerService} from "ngx-spinner";
import {Observable, of, Subscription} from "rxjs";
import {ActiveDeliveryResultDto} from "../../models/active-delivery-result-dto";
import {environment} from "@env";
import {ScanItemDeliveryCommand} from "../../api/delivery-api.service";
import {catchError, map} from "rxjs/operators";
import {ActivatedRoute} from "@angular/router";
import {ActiveDeliveryItem} from "../../models/active-delivery-item";
import {ActiveDeliveryScanResultDto} from "../../models/active-delivery-scan-result-dto";

@Component({
  selector: 'app-redeem-delivery',
  templateUrl: './redeem-delivery.component.html',
  styleUrls: ['./redeem-delivery.component.sass']
})
export class RedeemDeliveryComponent implements OnInit, AfterViewInit, OnDestroy {
  deliveryId: number
  deliveryName: string
  isLoading$: Observable<boolean>
  isDeleting$: Observable<boolean>
  deliveryInfo$: Observable<ActiveDeliveryResultDto>

  deliveryInfoSub: Subscription;

  deliveryConfigs = environment.delivery
  searchTerm: string
  selectedScanMode: boolean;

  successAudio: HTMLAudioElement;
  wrongAudio: HTMLAudioElement;

  constructor(private deliveryFacade: DeliveryFacade, private route: ActivatedRoute,
              private spinner: NgxSpinnerService) {
    this.isLoading$ = this.deliveryFacade.isLoading$()
    this.isDeleting$ = this.deliveryFacade.isDeleting$()
  }

  ngOnInit(): void {
    this.initSuccessAudio();
    this.initWrongAudio();
    this.deliveryName = history.state.deliveryName
    this.route.params.subscribe(params => {
      this.deliveryId = +params['id'];
      this.deliveryFacade.loadDeliveryForRedeem$(this.deliveryId).subscribe(res => {
        this.deliveryInfo$ = this.deliveryFacade.getDeliveryForRedeem$(this.deliveryId);
      });
    })
  }

  ngOnDestroy() {
    this.deliveryInfoSub?.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.spinner.show();
  }

  initSuccessAudio(): void {
    this.successAudio = new Audio("../../../../assets/sounds/success_scan.wav");
    this.successAudio.crossOrigin = 'anonymous'
    this.successAudio.volume = 0.5
    this.successAudio.load()
  }

  initWrongAudio(): void {
    this.wrongAudio = new Audio("../../../../assets/sounds/wrong_scan.wav");
    this.wrongAudio.crossOrigin = 'anonymous'
    this.wrongAudio.volume = 0.5
    this.wrongAudio.load()
  }

  scan(): void {
    console.log(this.deliveryId)

    this.deliveryFacade.scanDeliveryForRedeem(this.deliveryId, new ScanItemDeliveryCommand(this.searchTerm, this.selectedScanMode)).subscribe(res => {
      console.log(res)
      if ((<ActiveDeliveryScanResultDto>res).activeDeliveryInfo !== undefined)
        this.playSuccessAudio()
      else
        this.playWrongAudio()

      this.deliveryInfo$.pipe(map(delivery => {
        console.log(delivery)
        if (res && res.items && res.activeDeliveryInfo) {
          delivery.activeDeliveryInfo = res.activeDeliveryInfo
          delivery.items = res.items.map(res => {
            return new ActiveDeliveryItem(res, delivery.items.filter(i => i.id === res.id)[0].itemDescription)
          })
        }
      })).subscribe()
    });
  }

  redeemDelivery() : void {
    this.deliveryFacade.redeemDelivery(this.deliveryId).subscribe();
  }

  playSuccessAudio(): void {
    this.successAudio.play().then(function() {
      console.log('play success started')
    }).catch(function(error) {
      console.log(error)
    });
  }

  playWrongAudio(): void {
    this.wrongAudio.play().then(function() {
      console.log('play wrong started')
    }).catch(function(error) {
      console.log(error)
    });
  }
}
