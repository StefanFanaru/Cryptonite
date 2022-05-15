export interface ClientEvent {
  innerEventJson: any;
  createdAt: string;
  destination: ClientEventDestination;
}

export type ClientEventDestination = 'Toaster' | 'Dashboard';
